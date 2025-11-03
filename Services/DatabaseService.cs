using SQLite;
using MagazijnApp.Models;
using System;
using System.Security.Cryptography;
namespace MagazijnApp.Services;
using System.Text;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        try
        {
            // Use a simple relative path that works across platforms
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "products.db3");

            Console.WriteLine($"Database path: {dbPath}");
            Console.WriteLine($"Directory exists: {Directory.Exists(Path.GetDirectoryName(dbPath))}");

            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Products>().Wait();
            _database.CreateTableAsync<Users>().Wait();

            Console.WriteLine("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database initialization failed: {ex.Message}");
            throw; // Re-throw to see the error
        }
    }

    //Database didn't work and inventory page was unable to open with code below, debug and output console were empty?
    //to- do : fix this

    // string dbPath = "Database/products.db3";
    // Console.WriteLine($"Database path: {dbPath}");
    // Console.WriteLine($"Full path: {Path.GetFullPath(dbPath)}");
    // _database = new SQLiteAsyncConnection(dbPath);



    // _database = new SQLiteAsyncConnection("Database/products.db3");

    // Method below saved it to an appdata location, changed it to Database folder in project for easy access
    // _database = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "products.db3"));

    // FileSystem is a MAUI helper which gives you the proper folder for app data on any platform
    // Path.Combine gives you.a full path like C:/Users/You/AppData/your.app/products.db3
    // Creates a connection to a SQLite database file called products.db3 in the safe platform-specific app data directory


    // _database.CreateTableAsync<Products>().Wait();

    // This tells SQlite create a table that matches my Products class, the Wait() makes sure the table is created before you try to use it




    public async Task<bool> SaveProductAsync(Products product)
    {
        try
        {
            if (product.Id == 0)
            {
                // Insert new product and get the auto-generated ID
                await _database.InsertAsync(product);
            }
            else
            {
                // Update existing product
                await _database.UpdateAsync(product);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving product: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(Products product)
    {
        try
        {
            await _database.DeleteAsync(product);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting product: {ex.Message}");
            return false;
        }

        //Deletes a product using its ID, if it works > succes, if it fails it'll show why it failed
    }

    public async Task<List<Products>> GetAllProductsAsync()
    {
        return await _database.Table<Products>().ToListAsync();
    }
    //Gets all products from the database, puts them in a list and waits for the database to fully give all items in database before showing

    public async Task<Products> GetProductIdAsync(int id)
    {
        return await _database.Table<Products>()
        .Where(products => products.Id == id)
        .FirstOrDefaultAsync();
    }

    //product => product.Id == id is a lambda that checks each product wether the Id matches the given id
    //In short, search the specific product with the given Id
    //.FirstOrDefaultAsync searches the first row that matches the given Id

    public async Task<bool> RegisterUserAsync(string username, string password)
    {
        var existingUser = await _database.Table<Users>()
        .Where(u => u.UserName == username)
        .FirstOrDefaultAsync();

        if (existingUser != null)
        {
            //Username already taken
            return false;
        }

        Users newUser = new Users();
        newUser.UserName = username;

        if (username == "123456")
        {
            newUser.Role = "admin";
        }
        else
        {
            newUser.Role = "regular";
        }

        // Hash the password
        var (hash, salt) = PasswordHasher.HashPassword(password);
        newUser.PasswordHash = hash;
        newUser.Salt = salt;

        await _database.InsertAsync(newUser);
        return true;

    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var existingUser = await _database.Table<Users>()
        .Where(u => u.UserName == username)
        .FirstOrDefaultAsync();

        if (existingUser == null)
        {
            //Username not found
            return false;
        }
        else
        {
            if (PasswordHasher.VerifyPassword(password, existingUser.PasswordHash, existingUser.Salt ))
            {
                await Application.Current.MainPage.DisplayAlert("Debug", $"Role is: {existingUser.Role}", "OK");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

//https://chat.deepseek.com/a/chat/s/76276e8e-9091-41b5-a307-7eae89b8f8ba
