## Install Entity Framework
install-package EntityFramework


## Cascade Delete in Entity Framework Code-First
Cascade delete automatically deletes dependent records or set null to foreignkey properties when the principal record is deleted.
Cascade delete is enabled by default in Entity Framework for all types of relationships such as one-to-one, one-to-many and many-to-many.

Turn off cascading delete
Use Fluent API to configure entities to turn off the cascading delete as shown below.
modelBuilder.Entity<Student>()
            .HasOptional<Standard>(s => s.Standard)
            .WithMany()
            .WillCascadeOnDelete(false);




## Move Configurations to Separate Class in Code-First
When you have a large number of domain classes, then configuring every class in OnModelCreating can become unmanageable. Code-First enables you to move all the configurations related to one domain class to a separate class.

you can move all the configurations related to Student entity to a separate class which derives from EntityTypeConfiguration<TEntity>

Now, you can inform Fluent API about this class
// Moved all Student related configuration to StudentEntityConfiguration class
modelBuilder.Configurations.Add(new StudentEntityConfiguration());


## Database Initialization Strategies in Code-First
You already created a database after running your Code-First application the first time, but what about the second time onwards?? Will it create a new database every time you run the application? What about the production environment? How do you alter the database when you change your domain model? To handle these scenarios, you have to use one of the database initialization strategies

1. CreateDatabaseIfNotExists: This is default initializer. As the name suggests, it will create the database if none exists as per the configuration. However, if you change the model class and then run the application with this initializer, then it will throw an exception.
2. DropCreateDatabaseIfModelChanges: This initializer drops an existing database and creates a new database, if your model classes (entity classes) have been changed. So you don't have to worry about maintaining your database schema, when your model classes change.
3. DropCreateDatabaseAlways: As the name suggests, this initializer drops an existing database every time you run the application, irrespective of whether your model classes have changed or not. This will be useful, when you want fresh database, every time you run the application, like while you are developing the application.
4. Custom DB Initializer: You can also create your own custom initializer, if any of the above doesn't satisfy your requirements or you want to do some other process that initializes the database using the above initializer.


public class SchoolDBContext: DbContext 
{
        
    public SchoolDBContext(): base("SchoolDBConnectionString") 
    {
        Database.SetInitializer<SchoolDBContext>(new CreateDatabaseIfNotExists<SchoolDBContext>());

        //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
        //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseAlways<SchoolDBContext>());
        //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Standard> Standards { get; set; }
}

public class SchoolDBInitializer :  CreateDatabaseIfNotExists<SchoolDBContext>
{
    protected override void Seed(SchoolDBContext context)
    {
        base.Seed(context);
    }
}
   
## Turn off DB Initializer in Code-First
You can also turn off the DB initializer of your application. Suppose, for the production environment, you don't want to lose existing data, then you can turn off the initializer

public class SchoolDBContext: DbContext 
{
    public SchoolDBContext() : base("SchoolDBConnectionString")
    {            
        //Disable initializer
        Database.SetInitializer<SchoolDBContext>(null);
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Standard> Standards { get; set; }
}
