# Organization.Services.Customer

# Project Requirements
- SQS queue set up in AWS (optional)
- PostgresDB
- dotnet core 3.1
- Postman (or equivelant)
- dotnet migrations tooling (Note: The DB will require migrations to be run before calling an API)

# API requests
SelectAll customers:
`
GET: https://localhost:44360/customer/select-all?status=1
`

SelectSingle customers:
`
GET: https://localhost:44360/customer/select?contactId=d3a23269-72a0-4af8-a070-a45e87047c1b
`

Update customers:
`
POST: https://localhost:44360/customer/update
{
    "customer": {
        "customerId": "f9aa4394-46a3-471c-9e17-6ca44f0e550c",
        "status": 1,
        "createdAt": "2021-06-19T17:07:06",
        "updatedAt": "2021-06-19T17:07:12",
        "firstName": "Bob",
        "lastName": "Smith",
        "preferredName": "Bob",
        "dateOfBirth": "2021-06-19T17:07:27"
    }
}
`

Insert customers:
`
POST: https://localhost:44360/customer/insert
{
    "customer": {
        "customerId": "f9aa4394-46a3-471c-9e17-6ca44f0e550c",
        "status": 1,
        "createdAt": "2021-06-19T17:07:06",
        "updatedAt": "2021-06-19T17:07:12",
        "firstName": "Bob",
        "lastName": "Smith",
        "preferredName": "Bob",
        "dateOfBirth": "2021-06-19T17:07:27"
    },
    "contacts": [
        {
            "contactId": "925954fb-6673-452e-9ce8-9aaa2e52af9a",
            "customerId": "f9aa4394-46a3-471c-9e17-6ca44f0e550c",
            "contactType": 1,
            "contactEntry": "abcde@gmail.com",
            "isPrimaryContact": true,
            "requiresValidation": true
        }
    ]
}
`

SelectAll customer notes:
`
GET: https://localhost:44360/customer-note/select-all?customerId=d3a23269-72a0-4af8-a070-a45e87047c1b
`

Update customer notes:
`
POST: https://localhost:44360/customer-note/update
{
    "noteId": "75f2a82c-d27d-4553-b4e3-0c29bf50e1b5",
    "customerId": "d3a23269-72a0-4af8-a070-a45e87047c1b",
    "authoredDateTime": "2021-06-20T13:27:29",
    "contents": "Bye",
    "status": 1
}
`

Insert customer notes:
`
POST: https://localhost:44360/customer-note/insert
{
    "noteId": "75f2a82c-d27d-4553-b4e3-0c29bf50e1b5",
    "customerId": "d3a23269-72a0-4af8-a070-a45e87047c1b",
    "authoredDateTime": "2021-06-20T13:27:29",
    "contents": "Hello",
    "status": 2
}
`

A number of outstanding items still require attention and have been marked with TODO in the code.
Some of the more important items are outlined below:
- prevent sql injection
- complete unit tests
- configure AWS account for SES messaging, SQS, and a lambda to consume messages and send an email SES
- Remove sql from services and make non dependent on the postgres DB
- Fix .gitignore 
- Docker support
