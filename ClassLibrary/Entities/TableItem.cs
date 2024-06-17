namespace ClassLibrary.Entities
{
    public class TableItem
    {
        public TableItem(string name)
        {
            TableName = name;
        }
        public TableItem() { }

        public string TableName { get; set; }
    }
}

