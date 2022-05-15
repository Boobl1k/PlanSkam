import {Controller, Get, Post, Query} from "@nestjs/common";
import {UsersRepository} from "../Data/Repositories/Users.repository";

@Controller('users')
export class UsersController {
    constructor(private readonly usersRepo: UsersRepository) {
    }

    @Get('getAll')
    async getAll() {
        return this.usersRepo.GetAll();
    }

    @Get('isAuthor')
    async isAuthor(@Query('id') id: string) {
        return await this.usersRepo.IsAuthor(id);
    }

    @Post('makeAuthor')
    async makeAuthor(@Query('id') id: string) {
        return (await this.usersRepo.MakeAuthor(id))
            ? "User is now author"
            : "Error";
    }

    @Post('makeNotAuthor')
    async makeNotAuthor(@Query('id') id: string) {
        return (await this.usersRepo.MakeNotAuthor(id))
            ? "User is not author now"
            : "Error";
    }
}