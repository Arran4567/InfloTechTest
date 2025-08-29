# Startup

In order to use this project, the API project must also be running. This can be done by setting up configuring your Startup projects to use multiple projects, choosing to start both using IIS Express.

# Packages

One new package has been added to this project:
* BuildBundlerMinifier - Required for bundling

# Login

The login page currently uses the users seeded in the database, further work is required to implement passwords on created users.
To log in, the Username field uses the email of the user and the password is case-sensitive using the format "Surname123!".
E.g.
Username: ploew@example.com
Password: Loew123!
