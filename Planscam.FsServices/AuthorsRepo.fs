namespace Planscam.FsServices

open Microsoft.AspNetCore.Identity
open Planscam.DataAccess
open Planscam.Entities

type AuthorsRepo(dataContext: AppDbContext, userManager: UserManager<User>) =
    do()

