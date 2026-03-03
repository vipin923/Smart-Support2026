using Microsoft.ML.Data;

namespace Smart_Support2026
{
    public class ModelUser
    {
        public class TicketInput
        {
            [ColumnName(@"Description")] public string Description { get; set; }
            [ColumnName(@"Category")] public string Category { get; set; }
        }

        public class TicketPrediction
        {
            [ColumnName("PredictedLabel")]
            public string SelectedTag { get; set; } // The predicted category (e.g., "Hardware")
            public float[] Score { get; set; }      // Confidence scores for all categories
        }
    }
}
