import {EntityRepository, Repository} from "typeorm";
import {Genre} from "../Entities/Genre";

@EntityRepository(Genre)
export class GenresRepository extends Repository<Genre>{

}
