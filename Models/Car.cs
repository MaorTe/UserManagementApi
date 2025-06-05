using System.Text.Json.Serialization;

namespace UserManagementApi.Models;

public class Car
{
    public int Id { get; set; }              
    public string Company { get; set; }     
    public string Model { get; set; }        
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public ICollection<User> Users { get; set; }
}
