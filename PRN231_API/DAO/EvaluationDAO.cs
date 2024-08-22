// DAO/EvaluationDAO.cs
using PRN231_API.Models;
using Microsoft.EntityFrameworkCore;
using PRN231_API.DTO;


public class EvaluationDAO : IEvaluationDAO
{
    private readonly SchoolDBContext _context;

    public EvaluationDAO(SchoolDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EvaluationDTO>> GetEvaluationsByStudentIdAsync(int studentId)
    {
        var evaluations = await _context.Evaluations
            .Include(e => e.Student)  // Assuming `Student` is a navigation property
            .Include(e => e.Subject)  // Assuming `Subject` is a navigation property
            .Include(e => e.GradeType) // Assuming `GradeType` is a navigation property
            .Where(e => e.StudentId == studentId)
            .ToListAsync();

        var evaluationDTOs = evaluations.Select(e => new EvaluationDTO
        {
            EvaluationId = e.EvaluationId,
            StudentId = (int)e.StudentId,
            //StudentName = e.Student.Name, // Assuming `Name` is a property in `Student`
            SubjectId = e.Subject.SubjectId,
            //SubjectName = e.Subject.SubjectName, // Assuming `SubjectName` is a property in `Subject`
            GradeTypeId = e.GradeType.GradeTypeId,
            //GradeTypeName = e.GradeType.GradeTypeName, // Assuming `GradeTypeName` is a property in `GradeType`
            Grade = e.Grade,
            AdditionExplanation = e.AdditionExplanation
        }).ToList();

        return evaluationDTOs;
    }

    public async Task<Evaluation> GetEvaluationByIdAsync(int evaluationId)
    {
        return await _context.Evaluations
            .Include(e => e.Student)
            .Include(e => e.Subject)
            .Include(e => e.GradeType)
            .FirstOrDefaultAsync(e => e.EvaluationId == evaluationId);
    }


    public async Task<IEnumerable<Evaluation>> GetAllEvaluationsAsync()
    {
        return await _context.Evaluations.ToListAsync();
    }

    public async Task<Evaluation> AddEvaluationAsync(Evaluation evaluation)
    {
        _context.Evaluations.Add(evaluation);
        await _context.SaveChangesAsync();
        return evaluation;
    }

    public async Task UpdateEvaluationAsync(EvaluationDTO evaluationDTO)
    {
        var evaluation = await _context.Evaluations.FindAsync(evaluationDTO.EvaluationId);

        if (evaluation == null)
        {
            throw new KeyNotFoundException();
        }

        // Update properties
        evaluation.Grade = evaluationDTO.Grade;
        evaluation.AdditionExplanation = evaluationDTO.AdditionExplanation;

        await _context.SaveChangesAsync();
    }




    public async Task DeleteEvaluationAsync(int evaluationId)
    {
        var evaluation = await _context.Evaluations.FindAsync(evaluationId);
        if (evaluation != null)
        {
            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
        }
    }
}
