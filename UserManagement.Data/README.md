# New Packages

Two New packages have been added to this project:
* Microsoft.EntityFrameworkCore.SqlServer - Used for SQL Server DB connection
* Microsoft.EntityFrameworkCore.Tools

# Database

* The database has now been converted to use a SQL Server database rather than an in memory database and is now using mirgations.
* The database connection string can be modified in the appsettings.development.json of whichever project is connecting to the database (UserManagement.Api or UserManagement.Web).
