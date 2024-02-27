-- Create a new database called Petz
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Petz')
BEGIN
    CREATE DATABASE Petz;
END
GO

-- Select the database
USE Petz;
GO


-- Create tables

IF OBJECT_ID('People', 'U') IS NULL
BEGIN
    CREATE TABLE People (
        id INT,
        first_name VARCHAR(255),
        last_name VARCHAR(255),
        age INT
    );
END

IF OBJECT_ID('Pets', 'U') IS NULL
BEGIN
    CREATE TABLE Pets (
        id INT,
        name VARCHAR(255),
        breed VARCHAR(255),
        age INT,
        dead BIT
    );
END

IF OBJECT_ID('PersonPet', 'U') IS NULL
BEGIN
    CREATE TABLE PersonPet (
        person_id INT,
        pet_id INT
    );
END

-- Insert data into People

MERGE INTO People AS target
USING (VALUES
    (1, 'Barack', 'Obama', 52),
    (2, 'Charlie', 'Brown', 6),
    (3, 'Jon', 'Arbuckle', 63),
    (4, 'Phil', 'Winslow', 30),
    (5, 'George', 'Bush', 67),
    (6, 'Cathy', 'Hillman', 38)
) AS source (id, first_name, last_name, age)
ON target.id = source.id
WHEN NOT MATCHED THEN
    INSERT (id, first_name, last_name, age)
    VALUES (source.id, source.first_name, source.last_name, source.age);

-- Insert data into Pets

MERGE INTO Pets AS target
USING (VALUES
    (1, 'Marmaduke', 'Great Dane', 4, 0),
    (2, 'Barney', 'Scottish Terrier', 12, 1),
    (3, 'Bo', 'Dog', 5, 0),
    (4, 'Snoopy', 'Beagle', 5, 0),
    (5, 'Electra', 'Mixed Breed', 2, 0),
    (6, 'Odie', 'Beagle', 3, 0),
    (7, 'Sunny', 'Dog', 1, 0)
) AS source (id, name, breed, age, dead)
ON target.id = source.id
WHEN NOT MATCHED THEN
    INSERT (id, name, breed, age, dead)
    VALUES (source.id, source.name, source.breed, source.age, source.dead);

-- Insert data into PersonPet

MERGE INTO PersonPet AS target
USING (VALUES
    (1, 3),
    (1, 7),
    (2, 4),
    (6, 5),
    (3, 6),
    (4, 1),
    (5, 3)
) AS source (person_id, pet_id)
ON target.person_id = source.person_id AND target.pet_id = source.pet_id
WHEN NOT MATCHED THEN
    INSERT (person_id, pet_id)
    VALUES (source.person_id, source.pet_id);
