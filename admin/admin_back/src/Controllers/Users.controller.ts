import {Controller, Get} from "@nestjs/common";
import {UsersRepository} from "../Data/Repositories/Users.repository";

@Controller('users')
export class UsersController {
    constructor(private readonly usersRepo: UsersRepository) {
    }
    
    @Get('getAll')
    async GetAll() {
        return this.usersRepo.GetAll();
    }
}