CREATE DATABASE Part;

CREATE TABLE Item (
id integer PRIMARY KEY,
item_Name VARCHAR(50) NOT NULL,
parent_Item INTEGER NOT NULL 
FORIEGN KEY (parent_Item) REFERENCES Id (id),
cost INTEGER NOT NULL,
req_Date DATE NOT NULL,
);

INSERT INTO Item (Id, Item_Name, Parent_Item, Cost, Req_Date)
VALUES (1, 'Item' ,, 500 , '02-20-2024'),
       (2, 'Sub1' , 1 , 200 , '02-10-2024'),
	     (3, 'Sub2' , 2 , 300 , '01-05-2024'),
	     (4, 'Sub3' , 2 , '01-02-2024'),
	     (5, 'Sub4' , 2 , 400 , '01-02-2024'),
	     (6, 'Item2' ,, 600 , '03-15-2024'),
	     (7, 'Sub1' , 6 , 200 , '02-25-2024');
	  
