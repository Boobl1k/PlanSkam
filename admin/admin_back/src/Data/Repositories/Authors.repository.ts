import {EntityRepository, Repository} from "typeorm";
import {Author} from "../Entities/Author";

@EntityRepository(Author)
export class AuthorsRepository extends Repository<Author>{
    
}
