# EFCodeFirstAdvancedDemo
Entity Framework code first advanced features like cascade delete, move configuration, DB Initialization strategy, 
migration, EF power tools.

# Setup Solution
Install Entity Framework
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
You can also turn off the DB initializer of your application. Suppose, for the production environment, you don't want to lose 
existing data, then you can turn off the initializer

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

## Seed Database in Code-First
You can insert data into your database tables during the database initialization process. This will be important if you want to 
provide some test data for your application or to provide some default master data for your application.

To seed data into your database, you have to create custom DB initializer, as you created in DB Initialization Strategy, 
and override the Seed method.

public class SchoolDBInitializer : DropCreateDatabaseAlways<SchoolDBContext>
{
    protected override void Seed(SchoolDBContext context)
    {
        IList<Standard> defaultStandards = new List<Standard>();

        defaultStandards.Add(new Standard() { StandardName = "Standard 1", Description = "First Standard" });
        defaultStandards.Add(new Standard() { StandardName = "Standard 2", Description = "Second Standard" });
        defaultStandards.Add(new Standard() { StandardName = "Standard 3", Description = "Third Standard" });

        foreach (Standard std in defaultStandards)
            context.Standards.Add(std);

        base.Seed(context);
    }
}

public class SchoolContext: DbContext 
{
        
    public SchoolContext(): base("SchoolDBConnectionString") 
    {
        Database.SetInitializer(new SchoolDBInitializer());

    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Standard> Standards { get; set; }
}

## Migration in Code-First
Entity framework Code-First had different database initialization strategies prior to EF 4.3 like CreateDatabaseIfNotExists,
DropCreateDatabaseIfModelChanges, or DropCreateDatabaseAlways. However, there were some problems with these strategies, 
for example if you already have data (other than seed data) or existing Stored Procedures, triggers etc in your database, 
these strategies used to drop the entire database and recreate it, so you would lose the data and other DB objects.

Entity framework 4.3 has introduced a migration tool that automatically updates the database schema, when your model changes 
without losing any existing data or other database objects. It uses a new database initializer called MigrateDatabaseToLatestVersion

There are two kinds of Migration:
1. Automated Migration
2. Code based Migration

1. Automated Migration
Entity framework 4.3 has introduced Automated Migration so that you don't have to process database migration manually 
in the code file, for each change you make in your domain classes. You just need to run a command in Package Manger 
Console to accomplish this
** Run enable-migrations command

2. Code based Migration
Add-migration: It will scaffold the next migration for the changes you have made to your domain classes
Update-database: It will apply pending changes to the database based on latest scaffolding code file you created 
using "Add-Migration" command

Suppose you want to roll back the database schema to any of the previous states, then you can use "update-database" command with 
–TargetMigration parameter as shown below:

update-database -TargetMigration:"First School DB schema"
Use the "get-migration" command to see what migration has been applied.

