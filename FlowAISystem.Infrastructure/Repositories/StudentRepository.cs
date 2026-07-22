using FlowAISystem.Application.Interfaces.Repositories;
using FlowAISystem.Domain.Entities;
using FlowAISystem.Domain.ValueObjects;
using FlowAISystem.Infrastructure.Data;
using FlowAISystem.Shared.DTOs.Students;
using Microsoft.EntityFrameworkCore;

namespace FlowAISystem.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }
    // ==========================================
    // Student List
    // ==========================================
    public async Task<List<StudentListItemDto>> GetAllAsync(
    StudentSearchDto search)
    {
        IQueryable<Student> query = _context.Students
            .AsNoTracking()
            .Include(s => s.Department);

        // ==========================
        // Search
        // ==========================
        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            string term = search.SearchTerm.Trim().ToLower();

            query = query.Where(s =>
                s.StudentNumber.ToLower().Contains(term) ||
                s.Name.FirstName.ToLower().Contains(term) ||
                s.Name.LastName.ToLower().Contains(term) ||
                s.Email.ToLower().Contains(term));
        }

        // ==========================
        // Department Filter
        // ==========================
        if (search.DepartmentId.HasValue)
        {
            query = query.Where(s =>
                s.DepartmentId == search.DepartmentId.Value);
        }

        // Sorting starts here...

        // Sorting
        query = search.SortColumn switch
        {
            "StudentNumber" => search.SortDescending
                ? query.OrderByDescending(s => s.StudentNumber)
                : query.OrderBy(s => s.StudentNumber),

            "Department" => search.SortDescending
                ? query.OrderByDescending(s => s.Department!.Name)
                : query.OrderBy(s => s.Department!.Name),

            "EnrollmentDate" => search.SortDescending
                ? query.OrderByDescending(s => s.EnrollmentDate)
                : query.OrderBy(s => s.EnrollmentDate),

            _ => search.SortDescending
                ? query.OrderByDescending(s => s.Name.FirstName)
                : query.OrderBy(s => s.Name.FirstName)
        };

        return await query
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                FullName = s.Name.FullName,
                Email = s.Email,
                DepartmentName = s.Department != null
                    ? s.Department.Name
                    : "N/A",
                EnrollmentDate = s.EnrollmentDate
            })
            .ToListAsync();
    }

    // ==========================================
    // Student Details
    // ==========================================
    public async Task<StudentDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.Students
            .AsNoTracking()
            .Include(s => s.Department)
            .Where(s => s.Id == id)
            .Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                FirstName = s.Name.FirstName,
                LastName = s.Name.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                EnrollmentDate = s.EnrollmentDate,
                DepartmentId = s.DepartmentId,
                DepartmentName = s.Department != null
                    ? s.Department.Name
                    : "N/A"
            })
            .FirstOrDefaultAsync();
    }

    // ==========================================
    // Student Edit DTO
    // ==========================================
    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        return await _context.Students
            .AsNoTracking()
            .Include(s => s.Department)
            .Where(s => s.Id == id)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                FirstName = s.Name.FirstName,
                LastName = s.Name.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                EnrollmentDate = s.EnrollmentDate,
                DepartmentId = s.DepartmentId,
                DepartmentName = s.Department != null
                    ? s.Department.Name
                    : "N/A"
            })
            .FirstOrDefaultAsync();
    }

    // ==========================================
    // Create
    // ==========================================
    public async Task CreateAsync(CreateStudentDto dto)
    {
        var student = new Student
        {
            StudentNumber = dto.StudentNumber,
            Name = new PersonName(dto.FirstName, dto.LastName),
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            EnrollmentDate = dto.EnrollmentDate,
            DepartmentId = dto.DepartmentId
        };

        _context.Students.Add(student);

        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);

        await _context.SaveChangesAsync();
    }

    // ==========================================
    // Update
    // ==========================================
    public async Task UpdateAsync(UpdateStudentDto dto)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == dto.Id);

        if (student is null)
            return;

        student.StudentNumber = dto.StudentNumber;
        student.Name = new PersonName(dto.FirstName, dto.LastName);
        student.Email = dto.Email;
        student.PhoneNumber = dto.PhoneNumber;
        student.Gender = dto.Gender;
        student.DateOfBirth = dto.DateOfBirth;
        student.EnrollmentDate = dto.EnrollmentDate;
        student.DepartmentId = dto.DepartmentId;
        student.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    // ==========================================
    // Delete
    // ==========================================
    public async Task DeleteAsync(int id)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student is null)
            return;

        _context.Students.Remove(student);

        await _context.SaveChangesAsync();
    }

    // User
    //public async Task<Student?> GetByUserIdAsync(int userId)
    //{
    //    return await _context.Students
    //        .Include(s => s.Department)
    //        .Include(s => s.User)
    //        .FirstOrDefaultAsync(s => s.UserId == userId);
    //}
    public async Task<Student?> GetByUserIdAsync(int userId)
    {
        return await _context.Students
            .Include(s => s.Department)
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    // ==========================================
    // Departments
    // ==========================================
    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _context.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    // ==================================================
    // Reports
    // ==================================================

    public async Task<List<StudentListItemDto>> GetStudentReportAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .Include(s => s.Department)
            .OrderBy(s => s.StudentNumber)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                FullName = s.Name.FullName,
                Email = s.Email,
                DepartmentName = s.Department != null
                    ? s.Department.Name
                    : "N/A",
                EnrollmentDate = s.EnrollmentDate
            })
            .ToListAsync();
    }
    // ==================================================
    // AI
    // ==================================================

    public async Task<int> GetCountAsync()
    {
        return await _context.Students.CountAsync();
    }

    public async Task<List<StudentListItemDto>> GetRecentStudentsAsync(
    int count = 5)
    {
        return await _context.Students
            .AsNoTracking()
            .Include(s => s.Department)
            .OrderByDescending(s => s.EnrollmentDate)
            .Take(count)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                FullName = s.Name.FullName,
                Email = s.Email,
                DepartmentName = s.Department != null
                    ? s.Department.Name
                    : "N/A",
                EnrollmentDate = s.EnrollmentDate
            })
            .ToListAsync();
    }
    // ==================================================
    // Find by Numbers
    // ==================================================
    public async Task<StudentListItemDto?> FindByStudentNumberAsync(
    string studentNumber)
    {
        studentNumber = studentNumber.Trim().ToLower();

        return await _context.Students
            .AsNoTracking()
            .Include(s => s.Department)
            .Where(s =>
                s.StudentNumber.ToLower() == studentNumber)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                FullName = s.Name.FullName,
                Email = s.Email,
                DepartmentName = s.Department != null
                    ? s.Department.Name
                    : "N/A",
                EnrollmentDate = s.EnrollmentDate
            })
            .FirstOrDefaultAsync();
    }

    public async Task<StudentListItemDto?> FindByNameAsync(
        string name)
    {
        name = name.Trim().ToLower();


        return await _context.Students
            .AsNoTracking()
            .Include(s => s.Department)

            .Where(s =>
                s.Name.FirstName.ToLower()
                    .Contains(name)

                ||

                s.Name.LastName.ToLower()
                    .Contains(name))

            .Select(s => new StudentListItemDto
            {
                Id = s.Id,

                StudentNumber = s.StudentNumber,

                FullName =
                    s.Name.FirstName
                    + " "
                    + s.Name.LastName,

                Email = s.Email,

                DepartmentName =
                    s.Department != null
                    ? s.Department.Name
                    : "N/A",

                EnrollmentDate = s.EnrollmentDate
            })

            .FirstOrDefaultAsync();
    }
    // update img
    public async Task UpdateProfileImageAsync(
    int userId,
    string imagePath)
    {

        var student =
            await _context.Students
            .FirstOrDefaultAsync(
                s => s.UserId == userId);



        if (student == null)
            return;



        student.ProfileImage =
            imagePath;



        student.UpdatedAt =
            DateTime.UtcNow;



        await _context.SaveChangesAsync();

    }

    public async Task UpdateEntityAsync(
    Student student)
    {
        _context.Students.Update(student);

        await _context.SaveChangesAsync();
    }
}
