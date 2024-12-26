# C# Test Assessment

The goal of this task is to implement a simple ETL project in CLI that inserts data from a CSV into a single, flat table. While the task is straightforward, make sure you take your time reading it more than once.

Input CSV data file: https://drive.google.com/file/d/1l2ARvh1-tJBqzomww45TrGtIh5j8Vud4/view?usp=sharing. You can download and read it locally rather than remotely.

## Objectives

1. Import the data from the CSV into an MS SQL table. We only want to store the following columns:
    - `tpep_pickup_datetime`
    - `tpep_dropoff_datetime`
    - `passenger_count`
    - `trip_distance`
    - `store_and_fwd_flag`
    - `PULocationID`
    - `DOLocationID`
    - `fare_amount`
    - `tip_amount`
2. Set up a SQL Server database (local or cloud-based, as per your convenience).
3. Design a table schema that will hold the processed data; make sure you are using the proper data types.
4. Users of the table will perform the following queries; ensure your schema is optimized for them:
    - Find out which `PULocationId` (Pick-up location ID) has the highest tip_amount on average.
    - Find the top 100 longest fares in terms of `trip_distance`.
    - Find the top 100 longest fares in terms of time spent traveling.
    - Search, where part of the conditions is `PULocationId`.
5. Implement efficient bulk insertion of the processed records into the database.
6. Identify and remove any duplicate records from the dataset based on a combination of `pickup_datetime`, `dropoff_datetime`, and `passenger_count`. Write all removed duplicates into a `duplicates.csv` file.
7. For the `store_and_fwd_flag` column, convert any 'N' values to 'No' and any 'Y' values to 'Yes'.
8. Ensure that all text-based fields are free from leading or trailing whitespace.
9. Assume your program will be used on much larger data files. Describe in a few sentences what you would change if you knew it would be used for a 10GB CSV input file.
10. (nice to have) The input data is in the EST timezone. Convert it to UTC when inserting into the DB.

## Requirements

- Use C# as the primary programming language.
- Efficiency of data insertion into SQL Server.
- Assume the data comes from a potentially unsafe source.

## Deliverables

- Project Source code. You can put it into a public Github repository but **WITHOUT THIS** specs file.
- SQL scripts used for creating the database and tables.
- Number of rows in your table after running the program.
- Any comments on any assumptions made.

If you have any questions, don't hesitate to ask! The task isn't described in minute detail on purpose.
