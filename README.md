# SunamoSqlite

SQLite data layer over `System.Data.SQLite`.

## What it contains

- `DatabaseLayer` holds the connection and the map of column types.
- `SloupecDB` and `ColumnsDB` describe the columns of a table; `GeneratorSqLite` turns them into `CREATE TABLE`.
- `StoredProceduresSqlite` and `StoredProceduresSqliteI` are ready-made select, insert, update and delete helpers.
- `BaseRowTable` is the base of classes that represent one table row.
- `InsertedRows`, `ChangedRows`, `DeletedRows` and `SelectedRows` carry the results of the operations.

## Target frameworks

net10.0, net9.0, net8.0
