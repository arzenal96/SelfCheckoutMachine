Requirements to build and run the application:
- Some IDE: Preferably Visual Studio
- A local SqlServer instance with the name of MSSQLLocalDB (note: It's changeable in the appsettings.json file and it's not case sensitive).

Once you have the SqlServer instance, all you need to do is to run the following command inside the Package Manager Console ( in Visual Studio: Tools -> NuGet Packet Manager -> Package Manager Console ): Update-Database

It will generate the required database with the given tables and it will also populate the Bill table with the accepted currencies.

The logging is using the application's console. You can check it in the Output window of Visual Studio by selecting "SelfCheckoutMachine - ASP.NET Core Web Server" from the "Show output from" dropdown.
