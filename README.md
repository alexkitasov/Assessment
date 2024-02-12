# Magellan Aerospace Software Team Hiring Assignment

1. Create a PostgreSQL script that does the following 
    * Create a database called `Part`
    * Create a table named `item` in the `Part` database with the specification below

        |Column|Type|Nullable|Constraint|
        |------|----|--------|----------|
        |id|integer|not `null`|primary key
        |item_name|character varying (50)|not `null`
        |parent_item|integer|nullable|self-referential foreign key to id
        |cost|integer|not `null`
        |req_date|date|not `null`

    * Insert data into the `item` table to look like below
        |id|item_name|parent_item|cost|req_date|
        |--|---------|-----------|----|--------|
        |1|Item1|`null`|500|02-20-2024
        |2|Sub1|1|200|02-10-2024
        |3|Sub2|1|300|01-05-2024
        |4|Sub3|2|300|01-02-2024
        |5|Sub4|2|400|01-02-2024
        |6|Item2|`null`|600|03-15-2024
        |7|Sub1|6|200|02-25-2024
    * Write a function called `Get_Total_Cost` that returns the total cost of an item   by aggregating its own cost and cost of child item using the data above. Caller will supply the function with a value that exists in `item_name`. You can assume if `parent_item` is `null` then the value of `item_name` of that record is unique.<br> 
        Example:
        ```
        Get_Total_Cost('Sub1') returns null. 
        Explanation: Sub1's parent_item is not null

        Get_Total_Cost('Item1') returns 1700
        Explanation:
                        Item1 (500)
                    ┌───────┴───────┐
                Sub1 (200)      Sub2 (300)
            ┌───────┴───────┐
        Sub3 (300)      Sub4 (400)

        500 + 200 + 300 + 300 + 400 = 1700
        ```
    * Save the script as this will be used in the next portion. 

2. Clone the Visual Studio 2022 project called MagellanTest in the repository. The project will target .NET 6 and use the standard data provider (Npgsql) without Entity Framework. Here you will be creating a simple REST API to access the database created above. In the `ItemsController.cs` of the project, create the API endpoints that lets users do the following tasks.
    * Create a new record in the `item` table. User will supply the values for `item_name`, `parent_item`, `cost`, and `req_date` in json. The endpoint will return the `id` of the newly created record.
    * Query the `item` table by supplying the `id` of the record. The endpoint will return the `id`, `item_name`, `parent_item`, `cost`, and `req_date` in json if record exists.
    * Create an endpoint that calls the `Get_Total_Cost` function in previous step. User will supply the `item_name` for the function. The endpoint will return the value returned by the function. 

3. Place the script created in part 1 in the PostgreSqlScript folder of the project. Put the project in your GitHub repository. If you applied to this role through Indeed, you can respond to the original message with the link to your project repository. Otherwise, send the repository link to Gregory.Schmidt@magellan.aero titled LastName_FirstName_Magellan_software_team_test. 
