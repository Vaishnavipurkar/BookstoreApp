# Bookstore App

A simple web application for managing a bookstore built using ASP.NET Core MVC.

## Table of Contents
- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [Setup](#setup)
- [Running the Application](#running-the-application)
- [Usage](#usage)
- [Contributing](#contributing)

## Overview

The Bookstore App is a basic web application that allows users to perform CRUD operations on books in a bookstore. The application also includes search functionality and pagination.

## Tech Stack

- **ASP.NET Core MVC**: Web framework for building the application.
- **Entity Framework Core**: ORM for interacting with the database.
- **SQL Server**: Database management system.
- **Bootstrap**: For styling the HTML pages.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete books.
- **Pagination**: Display books in a paginated view.
- **Search Functionality**: Search books by Title, Author, or Genre.
- **Dynamic Search**: (To be implemented) Real-time search results as the user types.

## Setup

### Prerequisites

- .NET SDK (version 6.0 or later)
- SQL Server
- Visual Studio or any other C# IDE

### Installation

1. **Clone the Repository**

    ```bash
    git clone https://github.com/yourusername/bookstore-app.git
    cd bookstore-app
    ```

2. **Configure the Database**

    Update the connection string in `appsettings.json` with your SQL Server details.

3. **Apply Migrations**

    Run the following command to apply migrations and set up the database:

    ```bash
    dotnet ef database update
    ```

## Running the Application

### Using Visual Studio

1. Open the solution file `BookstoreApp.sln` in Visual Studio.
2. Ensure `BookstoreApp` is set as the startup project.
3. Press `F5` to run the application.

### Using .NET CLI

1. Navigate to the project directory:

    ```bash
    cd BookstoreApp
    ```

2. Run the application:

    ```bash
    dotnet run
    ```

3. Open your browser and navigate to `https://localhost:7122`.

## Usage

- **View Books**: Navigate to the Books page to view the list of books.
- **Create a New Book**: Click on "Create New" to add a new book.
- **Edit a Book**: Click on "Edit" next to a book to modify its details.
- **Details of a Book** : Click on "Details" next to book to see the book details.
- **Delete a Book**: Click on "Delete" next to a book to remove it.
- **Search Books**: Use the search box to find books by Title, Author, or Genre.



