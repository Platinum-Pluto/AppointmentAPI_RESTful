# Appointment Management API Documentation

## Setup Instructions

### Prerequisites
- .NET 7.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Initial Setup

1. Clone the repository
2. Update the connection string in `appsettings.json` to point to your SQL Server instance
3. Open a terminal in the project directory and run the following commands:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

The API will start running on `https://localhost:7001` (or a similar port I guess).

## Authentication

### Register a New User
**Endpoint:** POST `/api/auth/register`

**Request Body:**
```json
{
    "username": "username",
    "password": "password"
}
```

**Response:** Status 200 OK with success message

### Login
**Endpoint:** POST `/api/auth/login`

**Request Body:**
```json
{
    "username": "username",
    "password": "password"
}
```

**Response:** JWT token string

## Using Protected Endpoints

All appointment endpoints require authentication. Include the JWT token in the Authorization header:

```
Authorization: Bearer <your_jwt_token>
```

## Appointment Endpoints

### Create Appointment
**Endpoint:** POST `/api/appointments`

**Request Body:**
```json
{
    "patientName": "Platinum",
    "patientContact": "012345567891",
    "appointmentDateTime": "2025-01-15T14:30:00",
    "doctorId": 1
}
```

### Get All Appointments
**Endpoint:** GET `/api/appointments`

### Get Specific Appointment
**Endpoint:** GET `/api/appointments/{id}`

### Update Appointment
**Endpoint:** PUT `/api/appointments/{id}`

**Request Body:**
```json
{
    "patientName": "Pluto",
    "patientContact": "123456578991",
    "appointmentDateTime": "2025-01-15T14:30:00",
    "doctorId": 1
}
```

### Delete Appointment
**Endpoint:** DELETE `/api/appointments/{id}`

## Error Handling

The API returns appropriate HTTP status codes:
- 200: Success
- 201: Created
- 400: Bad Request (invalid input)
- 401: Unauthorized (invalid or missing token)
- 404: Not Found
- 500: Server Error

## Input Validation Rules

1. Appointment date must be in the future
2. Doctor ID must be valid
3. Patient name and contact information are required
4. Password must be at least 6 characters long

## Testing

### Running Unit Tests
```bash
cd AppointmentAPI.Tests
dotnet test
```

### Postman Collection
You can import the following requests into Postman:

1. Register User:
   - POST `{{baseUrl}}/api/auth/register`
   - Body: `{"username": "test", "password": "test123"}`

2. Login:
   - POST `{{baseUrl}}/api/auth/login`
   - Body: `{"username": "test", "password": "test123"}`

3. Create Appointment:
   - POST `{{baseUrl}}/api/appointments`
   - Auth: Bearer Token
   - Body: See example above

4. Get Appointments:
   - GET `{{baseUrl}}/api/appointments`
   - Auth: Bearer Token

## Security Features

1. Password Hashing: Uses BCrypt for secure password storage
2. JWT Authentication: Tokens expire after 24 hours
3. Input Validation: Prevents invalid data entry
4. Entity Framework: Prevents SQL injection
5. HTTPS: All endpoints require HTTPS

## Development Notes

- The API uses Entity Framework Core with SQL Server
- JWT tokens are valid for 24 hours
- The application includes comprehensive error handling
- Unit tests cover all major functionality
- Database migrations are included for easy setup