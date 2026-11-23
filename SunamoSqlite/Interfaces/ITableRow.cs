namespace SunamoSqlite.Interfaces;

    /// <summary>
    /// Operations of one table row; the set is complete, do not add more methods here.
    /// More methods would mostly duplicate the existing ones.
    /// </summary>
    public interface ITableRow
    {
        /// <summary>
        /// UNIQUE, PRIMARY KEY and similar constraints are checked by the caller.
        /// </summary>
        int InsertToTable() => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        InsertedRows InsertToTableInsertedRows() => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        /// <summary>
        /// UNIQUE, PRIMARY KEY and similar constraints are checked by the caller.
        /// </summary>
        ChangedRows UpdateInTable() => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        ChangedRows UpdateInTable(List<string> nazvySloupcu, List<object> hodnotaSloupcu) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        ChangedRows UpdateInTable(string nazevSloupce, object hodnotaSloupce) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        ChangedRows UpdateInTableTest() => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        ChangedRows UpdateInTableTest(List<string> nazvySloupcu, List<object> hodnotaSloupcu) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        ChangedRows UpdateInTableTest(string nazevSloupce, object hodnotaSloupce) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        void SelectInTable() => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        /// <summary>
        /// Only the first result is used
        /// </summary>
        /// <param name="nazvySloupcu"></param>
        /// <param name="hodnotaSloupcu"></param>
        void SelectInTable(List<string> nazvySloupcu, List<object> hodnotaSloupcu) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        void SelectInTable(string nazevSloupce, object hodnotaSloupce) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        /// <summary>
        /// Returns all rows found
        /// </summary>
        /// <param name="nazvySloupcu"></param>
        /// <param name="hodnotaSloupcu"></param>
        SelectedRows SelectInTableSelectedRows(List<string> nazvySloupcu, List<object> hodnotaSloupcu) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        SelectedRows SelectInTableSelectedRows(string nazevSloupce, object hodnotaSloupce) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        DataTable SelectInTableDataTable(List<string> nazvySloupcu, List<object> hodnotaSloupcu) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        DataTable SelectInTableDataTable(string nazevSloupce, object hodnotaSloupce) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        DeletedRows DeleteFromTable() => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        DeletedRows DeleteFromTable(List<string> nazvySloupcu, List<object> hodnotaSloupcu) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        DeletedRows DeleteFromTable(string nazevSloupce, object hodnotaSloupce) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        DeletedRows DeleteFromTableTest() => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        DeletedRows DeleteFromTableTest(List<string> nazvySloupcu, List<object> hodnotaSloupcu) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        DeletedRows DeleteFromTableTest(string nazevSloupce, object hodnotaSloupce) => throw new NotImplementedException("The implementation is missing in the sources of the application.");
        //bool ObsahujeTabulkaVSloupciHodnotu(string sloupec, object hodnota);
    }
