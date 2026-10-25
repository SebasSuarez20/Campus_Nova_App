using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Common.Services;
using CampusNova.Domain.Exceptions;
using CampusNova.Domain.Student.Interfaces;
using MediatR;

namespace CampusNova.Application.Student.Command.UpdateStudent
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, string>
    {
        private readonly IStudentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudentHandler(IStudentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var studentModel = CampusNova.Domain.Student.Model.Student.Update(request.Student);

                string? passwordHash = string.IsNullOrWhiteSpace(request.Student.Password)
                    ? null
                    : HashServices.Hash(request.Student.Password);

                await _repository.UpdateStudent(studentModel, passwordHash, cancellationToken);

                _unitOfWork.Commit();
                return "Tus datos fueron actualizados correctamente";
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new TransactionRollbackException(ex.Message);
            }
        }
    }
}
