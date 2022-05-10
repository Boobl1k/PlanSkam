import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

@Entity()
export class Genre {
    @PrimaryGeneratedColumn()
    Id: number;

    @Column()
    Name: string;

    @Column()
    PictureId: number;
}
