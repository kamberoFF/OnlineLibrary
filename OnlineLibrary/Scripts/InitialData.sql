-- Create roles
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES 
('1', 'Admin', 'ADMIN', NEWID()),
('2', 'User', 'USER', NEWID()),
('3', 'Author', 'AUTHOR', NEWID());

-- Assign admin role
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('4f61e6a5-81d9-4c71-9faa-e541f333481d', '1');

-- Create books
INSERT INTO Books (Id, Title, Author, AuthorId, Genre, Description, PublishedYear, Available, RegistrationDate)
VALUES 
('1', 'Harry Potter and the Philosopher''s Stone', 'J.K. Rowling', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Fantasy', 
'Harry Potter has never even heard of Hogwarts when the letters start dropping on the doormat at number four, Privet Drive. Addressed in green ink on yellowish parchment with a purple seal, they are swiftly confiscated by his grisly aunt and uncle. Then, on Harry''s eleventh birthday, a great beetle-eyed giant of a man called Rubeus Hagrid bursts in with some astonishing news: Harry Potter is a wizard, and he has a place at Hogwarts School of Witchcraft and Wizardry.', 
1997, 1, DATEADD(DAY, -60, GETDATE())),

('2', 'Harry Potter and the Chamber of Secrets', 'J.K. Rowling', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Fantasy', 
'Harry Potter''s summer has included the worst birthday ever, doomy warnings from a house-elf called Dobby, and rescue from the Dursleys by his friend Ron Weasley in a magical flying car! Back at Hogwarts School of Witchcraft and Wizardry for his second year, Harry hears strange whispers echo through empty corridors - and then the attacks start.', 
1998, 1, DATEADD(DAY, -59, GETDATE())),

('3', 'A Game of Thrones', 'George R.R. Martin', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Fantasy', 
'Long ago, in a time forgotten, a preternatural event threw the seasons out of balance. In a land where summers can last decades and winters a lifetime, trouble is brewing. The cold is returning, and in the frozen wastes to the north of Winterfell, sinister forces are massing beyond the kingdom''s protective Wall.', 
1996, 1, DATEADD(DAY, -45, GETDATE())),

('4', 'A Clash of Kings', 'George R.R. Martin', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Fantasy', 
'A comet the color of blood and flame cuts across the sky. Two great leaders—Lord Eddard Stark and Robert Baratheon—who hold sway over an age of enforced peace are dead, victims of royal treachery. Now, from the ancient citadel of Dragonstone to the forbidding shores of Winterfell, chaos reigns.', 
1998, 1, DATEADD(DAY, -44, GETDATE())),

('5', 'To Kill a Mockingbird', 'Harper Lee', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Fiction', 
'The unforgettable novel of a childhood in a sleepy Southern town and the crisis of conscience that rocked it. "To Kill A Mockingbird" became both an instant bestseller and a critical success when it was first published in 1960. It went on to win the Pulitzer Prize in 1961 and was later made into an Academy Award-winning film, also a classic.', 
1960, 1, DATEADD(DAY, -40, GETDATE())),

('6', '1984', 'George Orwell', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Science Fiction', 
'Among the seminal texts of the 20th century, Nineteen Eighty-Four is a rare work that grows more haunting as its futuristic purgatory becomes more real. Published in 1949, the book offers political satirist George Orwell''s nightmarish vision of a totalitarian, bureaucratic world and one poor stiff''s attempt to find individuality.', 
1949, 1, DATEADD(DAY, -38, GETDATE())),

('7', 'Pride and Prejudice', 'Jane Austen', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Romance', 
'Since its immediate success in 1813, Pride and Prejudice has remained one of the most popular novels in the English language. Jane Austen called this brilliant work "her own darling child" and its vivacious heroine, Elizabeth Bennet, "as delightful a creature as ever appeared in print."', 
1813, 1, DATEADD(DAY, -35, GETDATE())),

('8', 'The Great Gatsby', 'F. Scott Fitzgerald', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Fiction', 
'The Great Gatsby, F. Scott Fitzgerald''s third book, stands as the supreme achievement of his career. This exemplary novel of the Jazz Age has been acclaimed by generations of readers. The story is of the fabulously wealthy Jay Gatsby and his new love for the beautiful Daisy Buchanan, of lavish parties on Long Island at a time when The New York Times noted "gin was the national drink and sex the national obsession," it is an exquisitely crafted tale of America in the 1920s.', 
1925, 1, DATEADD(DAY, -30, GETDATE())),

('9', 'The Hobbit', 'J.R.R. Tolkien', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Fantasy', 
'In a hole in the ground there lived a hobbit. Not a nasty, dirty, wet hole, filled with the ends of worms and an oozy smell, nor yet a dry, bare, sandy hole with nothing in it to sit down on or to eat: it was a hobbit-hole, and that means comfort.', 
1937, 1, DATEADD(DAY, -28, GETDATE())),

('10', 'The Catcher in the Rye', 'J.D. Salinger', '4f61e6a5-81d9-4c71-9faa-e541f333481d', 'Fiction', 
'The hero-narrator of The Catcher in the Rye is an ancient child of sixteen, a native New Yorker named Holden Caulfield. Through circumstances that tend to preclude adult, secondhand description, he leaves his prep school in Pennsylvania and goes underground in New York City for three days.', 
1951, 1, DATEADD(DAY, -25, GETDATE()));

-- Create borrowed books (some returned, some active)
INSERT INTO BorrowedBooks (BookId, UserId, BorrowDate, ReturnDate)
VALUES 
('1', '4f61e6a5-81d9-4c71-9faa-e541f333481d', DATEADD(DAY, -20, GETDATE()), DATEADD(DAY, -10, GETDATE())),
('3', '4f61e6a5-81d9-4c71-9faa-e541f333481d', DATEADD(DAY, -15, GETDATE()), NULL),
('5', '4f61e6a5-81d9-4c71-9faa-e541f333481d', DATEADD(DAY, -5, GETDATE()), NULL),
('2', '4f61e6a5-81d9-4c71-9faa-e541f333481d', DATEADD(DAY, -18, GETDATE()), DATEADD(DAY, -8, GETDATE())),
('7', '4f61e6a5-81d9-4c71-9faa-e541f333481d', DATEADD(DAY, -12, GETDATE()), NULL);

-- Create reading history with reviews
INSERT INTO ReadingHistory (BookId, UserId, ReadDate, Rating, Review)
VALUES 
('1', '4f61e6a5-81d9-4c71-9faa-e541f333481d', DATEADD(DAY, -10, GETDATE()), 5, 'Absolutely loved this book! The magical world is so immersive and the characters are wonderful.'),
('2', '4f61e6a5-81d9-4c71-9faa-e541f333481d', DATEADD(DAY, -8, GETDATE()), 4, 'Great sequel that expands on the world building. Exciting plot with some surprising twists.');

-- Deletion queries
DELETE FROM Books
DELETE FROM BorrowedBooks
DELETE FROM ReadingHistory