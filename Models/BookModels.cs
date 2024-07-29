public class Book {
    public Guid BookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public bool Availability { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
