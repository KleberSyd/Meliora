-- Create tables

CREATE TABLE People (
    id INT,
    first_name VARCHAR(255),
    last_name VARCHAR(255),
    age INT
);

CREATE TABLE Pets (
    id INT,
    name VARCHAR(255),
    breed VARCHAR(255),
    age INT,
    dead BOOLEAN
);

CREATE TABLE PersonPet (
    person_id INT,
    pet_id INT
);

-- Insert data into People

INSERT INTO People (id, first_name, last_name, age) VALUES
(1, 'Barack', 'Obama', 52),
(2, 'Charlie', 'Brown', 6),
(3, 'Jon', 'Arbuckle', 63),
(4, 'Phil', 'Winslow', 30),
(5, 'George', 'Bush', 67),
(6, 'Cathy', 'Hillman', 38);

-- Insert data into Pets

INSERT INTO Pets (id, name, breed, age, dead) VALUES
(1, 'Marmaduke', 'Great Dane', 4, FALSE),
(2, 'Barney', 'Scottish Terrier', 12, TRUE),
(3, 'Bo', 'Dog', 5, FALSE),
(4, 'Snoopy', 'Beagle', 5, FALSE),
(5, 'Electra', 'Mixed Breed', 2, FALSE),
(6, 'Odie', 'Beagle', 3, FALSE),
(7, 'Sunny', 'Dog', 1, FALSE);

-- Insert data into PersonPet

INSERT INTO PersonPet (person_id, pet_id) VALUES
(1, 3),
(1, 7),
(2, 4),
(6, 5),
(3, 6),
(4, 1),
(5, 3);
