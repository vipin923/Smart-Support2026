using Microsoft.ML.Data;

namespace Smart_Support2026
{
    public class TicketsAI
    {
        public class TicketData
        {
            [ColumnName(@"TicketDescription")]
            public string TicketDescription {  get; set; }

            [ColumnName(@"ErrorType")]
            public string ErrorType { get; set; }

            [ColumnName(@"Priority")]
            public string Priority { get; set; }

            [ColumnName(@"SuggestedSolution")]
            public string SuggestedSolution { get; set; }

            [ColumnName(@"FixID")]
            public string FixID { get; set; }

        }
        public class TicketPrediction
        {
            [ColumnName("PredictedLabel")]
            public string ErrorType { get; set; }
        }


    }
}
