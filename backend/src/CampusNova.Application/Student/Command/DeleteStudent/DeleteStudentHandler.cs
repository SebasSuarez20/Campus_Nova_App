using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Exceptions;
using CampusNova.Domain.Student.Interfaces;
using MediatR;

namespace CampusNova.Application.Student.Command.DeleteStudent
{
    public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, string>
    {
        private readonly IStudentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudentHandler(IStudentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await _repository.DeleteStudent(cancellationToken);

                _unitOfWork.Commit();
                return "Tu registro fue eliminado correctamente";
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new TransactionRollbackException(ex.Message);
            }
        }
    }
}
