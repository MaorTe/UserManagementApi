using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementApi.Models;

public class User
{
    public int Id { get; set; }               
    public string Name { get; set; }           
    public string Email { get; set; }          
    public string Password { get; set; }       
    public DateTime CreatedAt { get; set; }    
    public DateTime UpdatedAt { get; set; }    

    // שדה הקשר לטבלה Cars (FK)
    // CarId מצביע למפתח הראשי של ה־Car
    public int? CarId { get; set; }            // אפשר שיהיה null אם למשתמש אין רכב
    [ForeignKey("CarId")]
    public Car Car { get; set; }               // ניווט אל רשומת ה־Car המשוייכת
}
