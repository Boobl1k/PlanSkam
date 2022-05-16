import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

@Entity('Authors')
export class Author {
    @PrimaryGeneratedColumn()
    Id: number;

    @Column()
    Name: string;

    @Column()
    PictureId: number;

    @Column()
    UserId: string;
}