CREATE TABLE Item (
id integer PRIMARY KEY,
item_Name VARCHAR(50) NOT NULL,
parent_Item INTEGER NOT NULL 
FORIEGN KEY (parent_Item) REFERENCES Id (id),
cost INTEGER NOT NULL,
req_Date DATE NOT NULL,
);

INSERT INTO Item (id, item_Name, parent_Item, cost, req_Date)
VALUES (1, 'Item1' ,, 500 , '02-20-2024'),
       (2, 'Sub1' , 1 , 200 , '02-10-2024'),
	   (3, 'Sub2' , 1 , 300 , '01-05-2024'),
	   (4, 'Sub3' , 2 , 300 ,'01-02-2024'),
	   (5, 'Sub4' , 2 , 400 , '01-02-2024'),
	   (6, 'Item2' ,, 600 , '03-15-2024'),
	   (7, 'Sub1' , 6 , 200, '02-25-2024');


CREATE OR REPLACE FUNCTION get_Total_Cost()

RETURN DECIMAL
AS
$total_Cost$
BEGIN
	WITH RECURSIVE item_Database AS (
		SELECT item_Name, cost
		FROM Item
		WHERE item_Name_ = item_Name
		UNION ALL
		SELECT i.item_Name, i.cost
		FROM Item i
		INNER JOIN item_Database iD ON i.parent_Item = iD.item_Name
		WHERE i.parent_Item IS NOT NULL
	)
	SELECT INTO total_Cost SUM(cost) FROM item_Database;

	RETURN total_Cost;
END;
$total_Cost$ LANGUAGE plpgsql;
