using CampusNova.Application.Subject.Command.EnrollSubject;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.Exceptions;
using CampusNova.Domain.Subject.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Subject.Command.DeleteSubject
{
    public class DeleteSubjectHandler : IRequestHandler<DeleteSubjectCommand, string>
    {

        private readonly ISubjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSubjectHandler(ISubjectRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await _repository.DeleteStudentSubject(request.SubjectCode, cancellationToken);

                _unitOfWork.Commit();
                return "Eliminada realizada correctamente";
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new TransactionRollbackException(ex.Message);
            }
        }

    }
}
