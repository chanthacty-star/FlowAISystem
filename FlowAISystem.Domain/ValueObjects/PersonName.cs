namespace FlowAISystem.Domain.ValueObjects;

public class PersonName
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    private PersonName()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public PersonName(string firstName, string lastName)
    {
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }
}