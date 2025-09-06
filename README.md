# Bookstore API

Simple .NET Web API for managing books. Uses XML file for storage.

## How to run

1. `dotnet run`
2. Go to `https://localhost:7000/swagger` to test

## Endpoints

- GET `/api/books` - get all books
- GET `/api/books/{isbn}` - get book by ISBN  
- POST `/api/books` - add new book
- PUT `/api/books/{isbn}` - update book
- DELETE `/api/books/{isbn}` - delete book
- GET `/api/books/report` - HTML report

## Notes

- Books stored in `books.xml` file
- ISBN is the unique identifier
- Supports multiple authors per book
- HTML report shows all books in table format
