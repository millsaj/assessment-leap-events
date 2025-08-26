# Install the dependencies
install: backend-install frontend-install

# Run the backend API
backend:
	dotnet run --project backend/Api/Api.csproj

# Run the frontend
frontend:
	cd frontend && npm run dev

# Run tests
test:
	cd backend/Api.Tests && dotnet test

# Install backend dependencies
backend-install:
	cd backend/Api && dotnet restore

# Install frontend dependencies
frontend-install:
	cd frontend && npm install

