using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCT.Models
{
    public abstract class Assessment
    {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public int CourseId { get; set; }
            public string? Title { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public abstract string GetAssessmentType();
    }

    public class PerformanceAssessment : Assessment
    {
        public override string GetAssessmentType() => "Performance Assessment";
    }

    public class ObjectiveAssessment : Assessment
    {
        public override string GetAssessmentType() => "Objective Assessment";
    }
}
