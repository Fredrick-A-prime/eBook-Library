

public class Transaction {
    public Guid TransactionID { get; set; }
    public Guid UserID { get; set; }
    public Guid BookID { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }

    public virtual User User { get; set; }
    public virtual Book Book { get; set; }
}
