using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Common.Services;
using CampusNova.Domain.Exceptions;
using CampusNova.Domain.Student.Interfaces;
using MediatR;

namespace CampusNova.Application.Student.Command.CreateStudent
{
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, string>
    {
        private readonly IStudentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudentHandler(IStudentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var studentModel = CampusNova.Domain.Student.Model.Student.Create(request.Student);
                var passwordHash = HashServices.Hash(request.Student.Password);

                await _repository.CreateStudent(studentModel, request.InstitutionCode, passwordHash, cancellationToken);

                _unitOfWork.Commit();
                return "Registro creado correctamente. Ya puedes iniciar sesión.";
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new TransactionRollbackException(ex.Message);
            }
        }
    }
}
