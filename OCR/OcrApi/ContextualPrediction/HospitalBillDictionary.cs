namespace OcrApi.ContextualPrediction;

public class HospitalBillDictionary
{
    public static List<string> GetValidWords()
    {
        return new List<string>
        {
            "Patient Name", "Admission Date", "Discharge Date", "Total Amount",
            "Consultation Fee", "Lab Tests", "Pharmacy", "Insurance ID",
            "Hospital Name", "Claim Amount"
        };
    }
}
