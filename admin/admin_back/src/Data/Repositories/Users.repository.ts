import {EntityRepository, Repository} from "typeorm";
import {User} from "../Entities/User";

@EntityRepository(User)
export class UsersRepository extends Repository<User>{
    async GetAll(){
        return await this.find();
    }
}