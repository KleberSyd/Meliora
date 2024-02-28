
# R University

## About The Project

R University is an interactive web application that demonstrates a dynamic counting mechanism, showcasing the seamless integration of educational features with the exploration of mathematical concepts. Developed using Blazor technology and styled with Bootstrap, this application provides a responsive and engaging user experience.

### Features

- **Dynamic Counting**: Counts from 1 to a user-defined limit.
- **Special Labels**: Marks multiples of 3 as "Nursing", multiples of 7 as "Meliora", and multiples of both 3 and 7 as "Nursing Meliora".
- **Interactivity**: Users can start, stop, and resume counting at any time.
- **Responsive Design**: Optimized for a smooth experience across all devices.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/download) or later
- A modern web browser
- Docker and Docker Compose installed on your machine. If you don't have them, follow the links below for installation:
  - [Install Docker](https://docs.docker.com/get-docker/)
  - [Install Docker Compose](https://docs.docker.com/compose/install/)

### Installation

1. Clone the repository
   ```sh
   git clone https://github.com/KleberSyd/Meliora.git
   ```
2. Navigate to the project directory
   ```sh
   cd Meliora
   ```
3. To build and start the application using Docker Compose, create a `docker-compose.yml` file in the project root with the following content:

   ```yaml
   version: '3.8'
   services:
     webapp:
       build: .
       ports:
         - "80:80"
       environment:
         - ASPNETCORE_ENVIRONMENT=Development
   ```

4. Run the application
   ```sh
   docker-compose up --build
   ```

   This will build the Docker image of the application (if necessary) and start a container based on that image.

5. Access the application in your browser at `http://localhost`.

## Technical Exercises and Requirements

This project also aims to address a set of technical exercises proposed in the "SON Analyst Programmer Technical Quiz", which includes creating a .NET solution, a business layer, a report page, SQL queries, and demonstrations of version control system usage. Each of these components plays a crucial role in assessing core competencies for the analyst/programmer position at the School of Nursing.

### .NET Solution (Meliora.sln)

The project includes a .NET solution that counts from 1 to 50, applying specific rules for multiples of 3 and 7, as detailed in the exercise requirements.

### Business Layer

A business layer was created to meet the needs of an entrepreneurial college student, providing functionalities to manage orders of cookies with various mix-ins.

### Report Page

A `Reports.aspx` page has been implemented to assist in order management and business planning, accessible only by the business owner.

### SQL Queries

SQL queries have been performed to analyze order data and customer preferences, using the AdventureWorks2016 database for demonstration.

### Version Control

A brief demonstration on the version control system
