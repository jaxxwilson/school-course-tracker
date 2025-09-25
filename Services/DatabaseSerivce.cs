using Plugin.LocalNotification;
using SCT.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCT.Services
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _db;

        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "sqliteDB.db");
            _db = new SQLiteAsyncConnection(dbPath);

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<PerformanceAssessment>();
            await _db.CreateTableAsync<ObjectiveAssessment>();
        }
        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();
            var terms = await _db.Table<Term>().ToListAsync();
            return terms;
        }
        public static async Task<Term> GetTerm(int courseTermId)
        {
            await Init();
            Term term = await _db.Table<Term>()
                .Where(t => t.Id == courseTermId)
                .FirstOrDefaultAsync();
            return term;
        }
        public static async Task<Term> AddTerm(string title, DateTime startDate, DateTime endDate)
        {
            await Init();
            var term = new Term()
            {
                Title = title,
                StartDate = startDate,
                EndDate = endDate
            };
            await _db.InsertAsync(term);
            return term;
        }
        public static async Task RemoveTerm(int id)
        {
            await Init();
            await _db.DeleteAsync<Term>(id);
        }
        public static async Task UpdateTerm(int id, string title, DateTime startDate, DateTime endDate)
        {
            await Init();
            var termQuery = await _db.Table<Term>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Title = title;
                termQuery.StartDate = startDate;
                termQuery.EndDate = endDate;

                await _db.UpdateAsync(termQuery);
            }
        }
        public static async Task RemoveCourses(int termId)
        {
            await Init();
            var deleteCourses = await GetCoursesForTerm(termId);
            foreach (Course course in deleteCourses)
            {
                await _db.DeleteAsync<Course>(course.Id);
            }
        }

        public static async Task<IEnumerable<Course>> GetCoursesForTerm(int termId)
        {
            try
            {
                await Init();
                var courses = await _db.Table<Course>()
                    .Where(e => e.TermId == termId)
                    .ToListAsync();

                return courses;
            }
            catch (Exception e)
            {
                string msg = e.Message.ToString();
                return new List<Course>();
            }
        }
        public static async Task<IEnumerable<Course>> GetCourses()
        {
            await Init();
            var courses = await _db.Table<Course>().ToListAsync();
            return courses;
        }
        public static async Task<Course> AddCourse(string title, int termId, DateTime startDate, DateTime endDate, string status, string instructor, string phone, string email, string notes = "")
        {
            await Init();
            var course = new Course()
            {
                Title = title,
                TermId = termId,
                StartDate = startDate,
                EndDate = endDate,
                Status = status,
                Instructor = instructor,
                Phone = phone,
                Email = email,
                Notes = notes
            };
            await _db.InsertAsync(course);
            return course;
        }
        public static async Task UpdateCourse(int id, string title, DateTime startDate, DateTime endDate, string status, string name, string phone, string email, string notes = "")
        {
            await Init();
            var courseQuery = await _db.Table<Course>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.Title = title;
                courseQuery.StartDate = startDate;
                courseQuery.EndDate = endDate;
                courseQuery.Status = status;
                courseQuery.Instructor = name;
                courseQuery.Phone = phone;
                courseQuery.Email = email;
                courseQuery.Notes = notes;

                await _db.UpdateAsync(courseQuery);
            }
        }
        public static async Task RemoveCourse(int courseId)
        {
            await Init();

            var PA = await _db.Table<PerformanceAssessment>()
                .Where(p => p.CourseId == courseId).ToListAsync();
            var OA = await _db.Table<ObjectiveAssessment>()
                .Where(o => o.CourseId == courseId).ToListAsync();

            foreach (var pa in PA)
                await _db.DeleteAsync(pa);
            foreach (var oa in OA)
                await _db.DeleteAsync(oa);

            await _db.DeleteAsync<Course>(courseId);
        }
        public static async Task<PerformanceAssessment> GetPerformanceAssessment(int courseId)
        {
            await Init();
            var pa = await _db.Table<PerformanceAssessment>()
                .Where(a => a.CourseId == courseId)
                .FirstOrDefaultAsync();
            return pa;
        }
        public static async Task<ObjectiveAssessment> GetObjectiveAssessment(int courseId)
        {
            await Init();
            var pa = await _db.Table<ObjectiveAssessment>()
                .Where(a => a.CourseId == courseId)
                .FirstOrDefaultAsync();
            return pa;
        }
        public static async Task<PerformanceAssessment> AddPAAssessment(int courseId, DateTime courseStartDate, DateTime courseEndDate)
        {
            await Init();

            var newPa = new PerformanceAssessment()
            {
                CourseId = courseId,
                Title = "PA Title",
                StartDate = courseStartDate,
                EndDate = courseEndDate
            };
            await _db.InsertAsync(newPa);
            return newPa;
        }
        public static async Task UpdatePAAssessment(int paId, string paTitle, DateTime paStartDate, DateTime paEndDate)
        {
            await Init();

            var paQuery = await _db.Table<PerformanceAssessment>()
                .Where(i => i.Id == paId)
                .FirstOrDefaultAsync();

            if (paQuery != null)
            {
                paQuery.Title = paTitle;
                paQuery.StartDate = paStartDate;
                paQuery.EndDate = paEndDate;
                await _db.UpdateAsync(paQuery);
            }
        }
        public static async Task UpdateOAAssessment(int oaId, string oaTitle, DateTime oaStartDate, DateTime oaEndDate)
        {
            await Init();

            var oaQuery = await _db.Table<ObjectiveAssessment>()
                .Where(i => i.Id == oaId)
                .FirstOrDefaultAsync();

            if (oaQuery != null)
            {
                oaQuery.Title = oaTitle;
                oaQuery.StartDate = oaStartDate;
                oaQuery.EndDate = oaEndDate;
                await _db.UpdateAsync(oaQuery);
            }
        }
        public static async Task DeletePAAssessment(PerformanceAssessment pa)
        {
            await Init();
            await _db.DeleteAsync(pa);
        }
        public static async Task<ObjectiveAssessment> AddOAAssessment(int courseId, DateTime courseStartDate, DateTime courseEndDate)
        {
            await Init();
            var oa = new ObjectiveAssessment()
            {
                CourseId = courseId,
                Title = "OA Title",
                StartDate = courseStartDate,
                EndDate = courseEndDate
            };
            await _db.InsertAsync(oa);
            return oa;
        }
        public static async Task DeleteOAAssessment(ObjectiveAssessment oa)
        {
            await Init();
            await _db.DeleteAsync(oa);
        }


        public static async Task ClearAll()
        {
            await ClearAllTerms();
            await ClearAllCourses();
            await ClearAssessments();
            LocalNotificationCenter.Current.ClearAll();
        }
        public static async Task ClearAllTerms()
        {
            await Init();
            await _db.DeleteAllAsync<Term>();
        }
        public static async Task ClearAllCourses()
        {
            await Init();
            await _db.DeleteAllAsync<Course>();
        }
        public static async Task ClearAssessments()
        {
            await Init();
            await _db.DeleteAllAsync<PerformanceAssessment>();
            await _db.DeleteAllAsync<ObjectiveAssessment>();
        }
    }
}
