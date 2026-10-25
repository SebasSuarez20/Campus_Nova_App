using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Exceptions;
using CampusNova.Domain.Subject.Interfaces;
using MediatR;

namespace CampusNova.Application.Subject.Command.EnrollSubject
{
    public class EnrollSubjectHandler : IRequestHandler<EnrollSubjectCommand, string>
    {
        private readonly ISubjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollSubjectHandler(ISubjectRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(EnrollSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await _repository.InsertStudentSubject(request.SubjectCode, cancellationToken);

                _unitOfWork.Commit();
                return "Inscripción realizada correctamente";
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new TransactionRollbackException(ex.Message);
            }
        }
    }
}
