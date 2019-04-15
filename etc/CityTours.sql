USE CityTours;
GO

--ALTER TABLE [dbo].[itinerary] DROP CONSTRAINT [fk_landmark_id];
--ALTER TABLE [dbo].[itinerary_user] DROP CONSTRAINT [fk_itinerary_id];
--ALTER TABLE [dbo].[itinerary_user] DROP CONSTRAINT [fk_user_id];

--DROP TABLE [dbo].[landmark];
--DROP TABLE [dbo].[users];
--DROP TABLE [dbo].[itinerary];
--DROP TABLE [dbo].[itinerary_user];

USE master;
GO

ALTER DATABASE CityTours SET MULTI_USER WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE IF EXISTS CityTours;
GO

CREATE DATABASE CityTours;
GO

USE CityTours;
GO

BEGIN TRANSACTION;

CREATE TABLE landmark (
    landmark_id integer identity NOT NULL,
    submitter_id integer NOT NULL,
    name varchar(100) NOT NULL,
    category varchar(50) NOT NULL,
    description varchar(500),
    address varchar(50),
    city varchar(50) NOT NULL,
    state varchar(2) NOT NULL,
    zip varchar(10) NOT NULL,
    latitude float NOT NULL,
    longitude float NOT NULL,
    hours_of_operation varchar(100),
    image_location varchar(500),

    CONSTRAINT pk_landmark_landmark_id PRIMARY KEY (landmark_id),

);

CREATE TABLE users (
    user_id integer identity NOT NULL,
    username varchar(50) NOT NULL,
    role varchar(50) NOT NULL,
    email varchar(100) NOT NULL,
    password varchar(100) NOT NULL,

    CONSTRAINT pk_user_user_id PRIMARY KEY (user_id),

);

CREATE TABLE itinerary (
    itinerary_id integer NOT NULL,
    landmark_id integer,
    visit_order integer,

	--CONSTRAINT pk_itinerary_itinerary_id PRIMARY KEY (itinerary_id),
    CONSTRAINT fk_landmark_id FOREIGN KEY (landmark_id) REFERENCES landmark(landmark_id),
);

CREATE TABLE itinerary_name (
	itinerary_id integer NOT NULL,
	itinerary_name varchar(100)
	CONSTRAINT pk_itinerary_name_itinerary_id PRIMARY KEY (itinerary_id)
)

CREATE TABLE itinerary_user (
    itinerary_id integer NOT NULL,
    user_id integer NOT NULL,

    --CONSTRAINT fk_itinerary_id FOREIGN KEY (itinerary_id) REFERENCES itinerary(itinerary_id),
	CONSTRAINT fk_user_id FOREIGN KEY (user_id) REFERENCES users(user_id),
);

SET IDENTITY_INSERT landmark ON;

INSERT INTO landmark (landmark_id, submitter_id, name, category, description, address, city, state, zip, latitude, longitude, hours_of_operation, image_location) VALUES (1, 1, 'Shrum Mound', 'Cemetery', 'Come and see one of the last ancient cone-shaped burial mounds remaining in Columbus, located in one-acre Campbell Park. Shrum Mound is a 20-foot-high and 100-foot-diameter mound built by people of the ancient Adena culture (800 B.C.–A.D. 100).', '3141 McKinley Ave', 'Columbus', 'OH', 43204, 39.989985, -83.080465, 'All Daylight Hours', 'Shrum_Mound_4_70000490.jpg');
INSERT INTO landmark (landmark_id, submitter_id, name, category, description, address, city, state, zip, latitude, longitude, hours_of_operation, image_location) VALUES (2, 2, 'Columbus Zoo and Aquarium', 'Zoo', 'Animal & marine life showcase with a hands-on tide pool, reptile lab, kangaroo walkabout & more.', '4850 W Powell Rd', 'Powell', 'OH', 43065, 40.156061, -83.118373, '9:00 AM - 5:00 PM daily', 'columbus-zoo-entrance-5296---g-jones-columbus-zoo-and-aquarium.jpg');
INSERT INTO landmark (landmark_id, submitter_id, name, category, description, address, city, state, zip, latitude, longitude, hours_of_operation, image_location) VALUES (3, 3, 'Franklin Park Conservatory and Botanical Gardens', 'Garden', 'You can explore a paradise of flora and fauna at the Franklin Park Conservatory and Botanical Gardens. The Conservatory features hundreds of species of plants from around the world in towering glass greenhouses. Walk through the rainforest, desert, orchid collection, and the grand Palm House, where you might spot a wedding in progress on the weekend.', '1777 E. Broad Street', 'Columbus', 'OH', 43203, 39.965358, -82.954723, '10:00 AM - 5:00 PM daily', 'ohio-columbus-franklin-park-conservatory-2.jpg');
INSERT INTO landmark (landmark_id, submitter_id, name, category, description, address, city, state, zip, latitude, longitude, hours_of_operation, image_location) VALUES (4, 1, 'Franklinton Centennial Boulder', 'Landmark', 'The Franklinton Centennial was held September 14-16, 1897. Events were held between Wheatland and Whitethorne Avenues near the Central Ohio Psychiatric Hospital grounds. The boulder has a fifty-one foot circumference and as tall as the average person. It is located on Eureka Avenue to the walking path on Dry Run Creek.', NULL, 'Columbus', 'OH', 43223, 39.96172, -83.061591, NULL, 'boulder.jpg');
INSERT INTO landmark (landmark_id, submitter_id, name, category, description, address, city, state, zip, latitude, longitude, hours_of_operation, image_location) VALUES (5, 2, 'Antrim Park', 'Park', 'This expansive community resource offers multiple sports fields & courts, plus walking trails.', '5800 Olentangy River Rd', 'Columbus', 'OH', 43085, 40.078503, -83.03783, '7:00 AM - 11:00 PM daily', 'antrim-park-walkway-jog-700x400.jpeg');
INSERT INTO landmark (landmark_id, submitter_id, name, category, description, address, city, state, zip, latitude, longitude, hours_of_operation, image_location) VALUES (6, 1, 'Insane Cemetery', 'Cemetery', 'Cemetery utilized by past psychiatric hospitals.', NULL, 'Columbus', 'OH', 43223, 39.962982, -83.06282, NULL, 'insane-cemetary.jpg');

SET IDENTITY_INSERT landmark OFF;

SET IDENTITY_INSERT users ON;

INSERT INTO users (user_id, username, role, email, password) VALUES (1, 'aleciberg', 'administrator', 'aleciberg@gmail.com', 'AlecIberg99!');
INSERT INTO users (user_id, username, role, email, password) VALUES (2, 'davidvanderburgh', 'administrator', 'davidvanderburgh@gmail.com', 'DavidVanderburgh99!');
INSERT INTO users (user_id, username, role, email, password) VALUES (3, 'kylethomas', 'administrator', 'kylethomas@gmail.com', 'KyleThomas99!');
INSERT INTO users (user_id, username, role, email, password) VALUES (4, 'mackenziejones', 'visitor', 'mackenziejones@gmail.com', 'MackenzieJones99!');

SET IDENTITY_INSERT users OFF;

INSERT INTO itinerary (itinerary_id, landmark_id, visit_order) VALUES (1, 2, 1);
INSERT INTO itinerary (itinerary_id, landmark_id, visit_order) VALUES (1, 1, 2);
INSERT INTO itinerary (itinerary_id, landmark_id, visit_order) VALUES (2, 1, 1);
INSERT INTO itinerary (itinerary_id, landmark_id, visit_order) VALUES (2, 3, 2);
INSERT INTO itinerary (itinerary_id, landmark_id, visit_order) VALUES (3, 2, 1);
INSERT INTO itinerary (itinerary_id, landmark_id, visit_order) VALUES (3, 4, 2);
INSERT INTO itinerary (itinerary_id, landmark_id, visit_order) VALUES (3, 5, 3);

INSERT INTO itinerary_user (itinerary_id, user_id) VALUES (1, 2);
INSERT INTO itinerary_user (itinerary_id, user_id) VALUES (2, 4);
INSERT INTO itinerary_user (itinerary_id, user_id) VALUES (3, 1);

INSERT INTO itinerary_name (itinerary_id, itinerary_name) VALUES (1, 'Saturday Trip!')
INSERT INTO itinerary_name (itinerary_id, itinerary_name) VALUES (2, 'Family Fun')
INSERT INTO itinerary_name (itinerary_id, itinerary_name) VALUES (3, 'First Date')

COMMIT;