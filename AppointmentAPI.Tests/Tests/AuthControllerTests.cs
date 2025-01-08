using Xunit;
using Microsoft.EntityFrameworkCore;
using AppointmentAPI.Controllers;
using AppointmentAPI.Data;
using AppointmentAPI.DTOs;
using AppointmentAPI.Services;
using AppointmentAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentAPI.Tests
{
    public class AuthControllerTests : IDisposable
    {
        private readonly AppointmentDbContext _context;
        private readonly AuthController _controller;
        private readonly JwtService _jwtService;

        public AuthControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppointmentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppointmentDbContext(options);
            
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"JWT:Key", ""} 
                })
                .Build();

            _jwtService = new JwtService(configuration);
            _controller = new AuthController(_context, _jwtService);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Register_WithValidUser_ReturnsOkResult()
        {
        
            var userDto = new UserDto
            {
                Username = "testuser",
                Password = "testpassword123"
            };

      
            var result = await _controller.Register(userDto);

       
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("User registered successfully", okResult.Value);
        }

        [Fact]
        public async Task Register_WithExistingUsername_ReturnsBadRequest()
        {
     
            var userDto = new UserDto
            {
                Username = "existinguser",
                Password = "password123"
            };

            await _controller.Register(userDto);

       
            var result = await _controller.Register(userDto);

         
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Username already exists", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_WithEmptyUsername_ReturnsBadRequest()
        {
           
            var userDto = new UserDto
            {
                Username = "",
                Password = "password123"
            };

       
            var result = await _controller.Register(userDto);

       
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Register_WithEmptyPassword_ReturnsBadRequest()
        {
          
            var userDto = new UserDto
            {
                Username = "testuser",
                Password = ""
            };

          
            var result = await _controller.Register(userDto);

        
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
        
            var userDto = new UserDto
            {
                Username = "loginuser",
                Password = "password123"
            };

            await _controller.Register(userDto);

            
            var result = await _controller.Login(userDto);

           
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
            Assert.IsType<string>(okResult.Value);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsBadRequest()
        {
           
            var userDto = new UserDto
            {
                Username = "loginuser2",
                Password = "password123"
            };

            await _controller.Register(userDto);

            var loginDto = new UserDto
            {
                Username = "loginuser2",
                Password = "wrongpassword"
            };

           
            var result = await _controller.Login(loginDto);

         
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid password", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_WithNonexistentUser_ReturnsBadRequest()
        {
          
            var userDto = new UserDto
            {
                Username = "nonexistentuser",
                Password = "password123"
            };

         
            var result = await _controller.Login(userDto);

         
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("User not found", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_WithEmptyUsername_ReturnsBadRequest()
        {
         
            var userDto = new UserDto
            {
                Username = "",
                Password = "password123"
            };

           
            var result = await _controller.Login(userDto);

      
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Login_WithEmptyPassword_ReturnsBadRequest()
        {
      
            var userDto = new UserDto
            {
                Username = "testuser",
                Password = ""
            };

           
            var result = await _controller.Login(userDto);

       
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Register_ThenLogin_WithSameCredentials_Succeeds()
        {
        
            var userDto = new UserDto
            {
                Username = "completeflowuser",
                Password = "password123"
            };

          
            await _controller.Register(userDto);
            var loginResult = await _controller.Login(userDto);

        
            var okResult = Assert.IsType<OkObjectResult>(loginResult.Result);
            Assert.NotNull(okResult.Value);
            Assert.IsType<string>(okResult.Value);
        }
    }
}