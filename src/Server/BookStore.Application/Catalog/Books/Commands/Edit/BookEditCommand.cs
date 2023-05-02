﻿namespace BookStore.Application.Catalog.Books.Commands.Edit;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.Exceptions;
using Application.Common.Models;
using Common;
using Domain.Catalog.Models.Books;
using Domain.Catalog.Repositories;
using Domain.Common.Models;
using MediatR;

public class BookEditCommand : BookCommand<BookEditCommand>, IRequest<Result<int>>
{
    public class BookEditCommandHandler : IRequestHandler<BookEditCommand, Result<int>>
    {
        private readonly IMemoryDatabase memoryDatabase;
        private readonly IBookDomainRepository bookRepository;
        private readonly IAuthorDomainRepository authorRepository;

        public BookEditCommandHandler(
            IMemoryDatabase memoryDatabase,
            IBookDomainRepository bookRepository,
            IAuthorDomainRepository authorRepository)
        {
            this.memoryDatabase = memoryDatabase;
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
        }

        public async Task<Result<int>> Handle(
            BookEditCommand request,
            CancellationToken cancellationToken)
        {
            var book = await this.bookRepository.Find(
                request.Id,
                cancellationToken);

            if (book is null)
            {
                throw new NotFoundException(nameof(book), request.Id);
            }

            var author = await this.authorRepository.Find(
                request.Author,
                cancellationToken);

            if (author is null)
            {
                throw new NotFoundException(nameof(author), request.Author);
            }

            book
                .UpdateTitle(request.Title)
                .UpdatePrice(request.Price)
                .UpdateQuantity(request.Quantity)
                .UpdateImageUrl(request.ImageUrl)
                .UpdateDescription(request.Description)
                .UpdateGenre(Enumeration.FromValue<Genre>(request.Genre))
                .UpdateAuthor(author);

            await this.bookRepository.Save(book, cancellationToken);

            await this.memoryDatabase.Remove("books:search");

            await this.memoryDatabase.AddOrUpdate("books:" + book.Id, book);

            return book.Id;
        }
    }
}