This project is the refactoring of Xero project "Refactor-this".

Review comments/onservations on the "refactor-this" project
===========================================================

Database scheme observations:
- Primary keys for "Product" and "ProductOption" use "uniqueidentifier".
  It seems this scheme is not designed to cater to clustered scenarios as by
  default primary key would be used as clustering key.
  Though the performance impact might be only during insert/delete operations
  this can be considered low priority and be revisited if we see any problems.
  A suggestion would be to consider default to "newsequentialid".

- "ProductId" column in "ProductOptions" table is not a foreign key.
  This has implications on storage when we have millions of rows in this table
  as data would be duplicated.
  At least we should consider adding a new index on this column.
  Note: As this is a "uniqueidentifier" it is advised non-clustered index.

Source code observations:
- It looks like the presentation layer (Controllers) is tightly coupled to
  the domain. This is okay if it is a small project, short lived but we should consider
  separating by introducing a service layer.
- Class naming "Helpers", "Products" and the code seem quite generic, it
  contains lot of business logic, acts like data access layer.
- SQL connections are being opened but not closed, these are expensive operations
  Also does not use prepared statements, so open to SQL injection attack.
- "Products" contains all the logic to connect to database, run sql commands, perform
  checks. Here we can consider adding DAO layer.
- As discussed above as "ProductId" is not a foreign key there are couple of bugs
  when creating product option we do not check if the product exists or not etc.
- Missing tests


Summary of changes:
===================
The refactoring theme is to provide layered architecture.

- Added non-clustered index on "ProductId" column of "ProductOption" table
- ProductRepository takes care of keeping domain objects away from database access code.
  It uses entity framework.
- The service layer captures the business logic, validations etc.
- The API controller takes care of web interface to access the service.
- Our use case yet does not require use of DTOs and model object could suffice,
  but in the best practice (for ex. maybe to reduce number of calls (DTO that encapsulates
  more information), reduce the number of exposed fields, remoting etc.)
- Add logging
- Add tests

Further (Not Done, but could be added later on):
- We could add pagination support when retrieving Products/ProductOptions
- Revisit depending on the scale at which this project will be used to see if we need
  to make changes to database.