# TwoHundred.TechTask

## Task description:

#### Create backend API that:

 1. uses a simple domain model representing companies and contracts:

- company must have a type: supplier/vendor
- company must have a list of historical addresses with date when it changed

2. exposes API 's that allows simple crud operations on companies

- exposes API that allows 'signing' of a contract between 2 companies

- contract should be guarded by some logic checks e.g., that one of the companies must be a supplier and the other a vendor

- when the contract is signed, a notification should be published and handled by a contract history logger in a decoupled way (can just add rows to a table in the same db)

3. unit tests, at least for the following:
- company update
- contract signing

Technologies to use: .net7, ms sql, cqrs, libraries of your choice