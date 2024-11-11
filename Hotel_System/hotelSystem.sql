
CREATE DATABASE IF NOT EXISTS hotelSystem;
USE hotelSystem;


-- Create Table `clients`
CREATE TABLE clients (
  id INT AUTO_INCREMENT PRIMARY KEY,
  first_name VARCHAR(100) NOT NULL,
  last_name VARCHAR(100) NOT NULL,
  phone VARCHAR(100) NOT NULL,
  country VARCHAR(100) NOT NULL
);
-- Insert data into `clients`
INSERT INTO clients (first_name, last_name, phone, country) VALUES
('pera', 'mikic', '0658526858', 'serbia'),
('fname', 'lname', '191191191', 'usa'),
('new', 'user', '+3817458948', 'macedonia');



-- Create Table `rooms_type`
CREATE TABLE rooms_type (
  id INT AUTO_INCREMENT PRIMARY KEY,
  label VARCHAR(200) NOT NULL,
  price DECIMAL(9,2) NOT NULL
);

-- Insert data into `rooms_type`
INSERT INTO rooms_type (label, price) VALUES
('single', 1000.00),
('double', 2000.00),
('family', 3000.00),
('suite', 5000.00);


-- Create Table `rooms`
CREATE TABLE rooms (
  number INT PRIMARY KEY,
  type INT NOT NULL,
  phone VARCHAR(100) NOT NULL,
  free VARCHAR(10) NOT NULL,
  FOREIGN KEY (type) REFERENCES rooms_type(id)
);

-- Insert data into `rooms`
INSERT INTO rooms (number, type, phone, free) VALUES
(2, 1,  '19605152625', 'YES'),
(3, 2,  '196051582616', 'NO'),
(5, 3,  '+381697458695', 'NO'),
(9, 2, '126495626', 'YES'),
(25, 4, '+3817485968', 'NO');

INSERT INTO rooms (number, type, phone, free) VALUES
(30, 1, '123-456-7890', 'YES'),
(31, 1, '123-456-7891', 'YES'),
(32, 2, '123-456-7892', 'YES'),
(33, 2, '123-456-7893', 'yES'),
(40, 3, '123-456-7894', 'YES'),
(35, 3, '123-456-7895', 'YES'),
(36, 4, '123-456-7896', 'YES'),
(37, 4, '123-456-7897', 'YES'),
(38, 1, '123-456-7898', 'YES'),
(39, 2, '123-456-7899', 'YES');


-- Create Table `reservations`
CREATE TABLE reservations (
  id INT AUTO_INCREMENT PRIMARY KEY,
  room_number INT,
  client_id INT,
  date_in DATE,
  date_out DATE,
  FOREIGN KEY (client_id) REFERENCES clients(id) ON DELETE CASCADE,
  FOREIGN KEY (room_number) REFERENCES rooms(number) ON DELETE CASCADE
);

-- Insert data into `reservations`
INSERT INTO reservations (room_number, client_id, date_in, date_out) VALUES
(2, 1, '2024-05-29', '2024-05-30'),
(3, 3, '2024-05-27', '2024-05-29');


-- Create Table `users`
CREATE TABLE users (
  id INT AUTO_INCREMENT PRIMARY KEY,
  username VARCHAR(50) NOT NULL,
  password VARCHAR(50) NOT NULL,
  role VARCHAR(50) NOT NULL,
  UNIQUE KEY (username)
);

-- Insert data into `users`
INSERT INTO users (username, password, role) VALUES
('admin', 'admin','Admin');

-- Create Table `employees`
CREATE TABLE employees (
  id INT AUTO_INCREMENT PRIMARY KEY,
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  phone_number VARCHAR(50) NOT NULL,
  position VARCHAR(50) NOT NULL,
  salary DECIMAL(9,2) NOT NULL,
  hire_date DATE NOT NULL
);

drop table employees;
-- Insert data into `employees`
INSERT INTO employees (first_name, last_name, phone_number, position, salary, hire_date) VALUES
('Abebe', 'Debebe', '123-456-7890', 'Manager', 5000.00, '2020-01-15'),
('Bethelhem', 'Mesfin', '234-567-8901', 'Receptionist', 3000.00, '2021-03-22'),
('Desta', 'Yosef', '345-678-9012', 'Housekeeper', 2500.00, '2019-08-01');


-- Views
CREATE VIEW view_reservations AS
SELECT reservations.id AS reservation_id, rooms.number AS room_number, clients.first_name, clients.last_name, reservations.date_in, reservations.date_out
FROM reservations
JOIN rooms ON reservations.room_number = rooms.number
JOIN clients ON reservations.client_id = clients.id;



CREATE VIEW view_room_details AS
SELECT rooms.number AS room_number, rooms_type.label AS room_type, rooms.phone AS room_phone, rooms.free AS room_availability, 
       reservations.id AS reservation_id, reservations.date_in, reservations.date_out, 
       clients.first_name AS client_first_name, clients.last_name AS client_last_name
FROM rooms
LEFT JOIN rooms_type ON rooms.type = rooms_type.id
LEFT JOIN reservations ON rooms.number = reservations.room_number
LEFT JOIN clients ON reservations.client_id = clients.id;


CREATE VIEW view_reservation_summary AS
SELECT 
    (SELECT COUNT(*) FROM reservations WHERE date_in >= CURDATE()) AS total_reservations,
    (SELECT COUNT(*) FROM reservations WHERE CURDATE() BETWEEN date_in AND date_out) AS active_reservations,
    (SELECT COUNT(*) FROM reservations WHERE date_in > CURDATE()) AS upcoming_reservations;



CREATE VIEW view_client_countries AS
SELECT country, COUNT(*) AS client_count
FROM clients
GROUP BY country;

-- procedures
-- insert clients

DELIMITER //

CREATE PROCEDURE InsertClient(
    IN first_name VARCHAR(50),
    IN last_name VARCHAR(50),
    IN phone VARCHAR(20),
    IN country VARCHAR(50)
)
BEGIN
    INSERT INTO clients (first_name, last_name, phone, country)
    VALUES (first_name, last_name, phone, country);
END //

DELIMITER ;


-- update clients
DELIMITER //

CREATE PROCEDURE UpdateClient(
    IN client_id INT,
    IN first_name VARCHAR(50),
    IN last_name VARCHAR(50),
    IN phone VARCHAR(20),
    IN country VARCHAR(50)
)
BEGIN
    UPDATE clients
    SET first_name = first_name,
        last_name = last_name,
        phone = phone,
        country = country
    WHERE id = client_id;
END //

DELIMITER ;

-- delete client
DELIMITER //

CREATE PROCEDURE DeleteClient(
    IN client_id INT
)
BEGIN
    DELETE FROM clients WHERE id = client_id;
END //

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE InsertRoom (
    IN room_number INT,
    IN room_type INT,
    IN room_phone VARCHAR(255),
    IN room_free VARCHAR(3)
)
BEGIN
    INSERT INTO rooms (number, type, phone, free)
    VALUES (room_number, room_type, room_phone, room_free);
END $$

DELIMITER $$
CREATE PROCEDURE UpdateRoom (
    IN room_number INT,
    IN room_type INT,
    IN room_phone VARCHAR(255),
    IN room_free VARCHAR(3)
)
BEGIN
    UPDATE rooms
    SET type = room_type, phone = room_phone, free = room_free
    WHERE number = room_number;
END $$

DELIMITER $$
CREATE PROCEDURE DeleteRoom (
    IN room_number INT
)
BEGIN
    DELETE FROM rooms WHERE number = room_number;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE InsertEmployee (
    IN first_name VARCHAR(255),
    IN last_name VARCHAR(255),
    IN phone_number VARCHAR(255),
    IN position VARCHAR(255),
    IN salary DECIMAL(10,2),
    IN hire_date DATE
)
BEGIN
    INSERT INTO employees (first_name, last_name, phone_number, position, salary, hire_date)
    VALUES (first_name, last_name, phone_number, position, salary, hire_date);
END $$

CREATE PROCEDURE UpdateEmployee (
    IN emp_id INT,
    IN first_name VARCHAR(255),
    IN last_name VARCHAR(255),
    IN phone_number VARCHAR(255),
    IN position VARCHAR(255),
    IN salary DECIMAL(10,2),
    IN hire_date DATE
)
BEGIN
    UPDATE employees
    SET first_name = first_name, last_name = last_name, phone_number = phone_number, 
        position = position, salary = salary, hire_date = hire_date
    WHERE id = emp_id;
END $$

CREATE PROCEDURE DeleteEmployee (
    IN emp_id INT
)
BEGIN
    DELETE FROM employees WHERE id = emp_id;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE InsertReservation (
    IN room_number INT,
    IN client_id INT,
    IN date_in DATE,
    IN date_out DATE
)
BEGIN
    INSERT INTO reservations (room_number, client_id, date_in, date_out)
    VALUES (room_number, client_id, date_in, date_out);
END $$

CREATE PROCEDURE UpdateReservation (
    IN reservation_id INT,
    IN room_number INT,
    IN client_id INT,
    IN date_in DATE,
    IN date_out DATE
)
BEGIN
    UPDATE reservations
    SET room_number = room_number, client_id = client_id, date_in = date_in, date_out = date_out
    WHERE id = reservation_id;
END $$

CREATE PROCEDURE DeleteReservation (
    IN reservation_id INT
)
BEGIN
    DELETE FROM reservations WHERE id = reservation_id;
END $$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE InsertUser (
    IN username VARCHAR(255),
    IN password VARCHAR(255),
    IN role VARCHAR(255)
)
BEGIN
    INSERT INTO users (username, password, role) VALUES (username, password, role);
END $$

DELIMITER ;






-- ALTER DATABASE hotelSystem CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;




-- Commit the transaction
COMMIT;
