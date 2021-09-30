import Pouchdb from "pouchdb-browser";

const db = new Pouchdb("perla-songs");

db.info(console.log);

/**
 * 
 * @param {FileList} files 
 */
export async function SaveFiles(files) {
    return await Promise.all(
        Array
            .from(files)
            .map(async file => await db.post({ name: file.name, attachments: { song: file } }))
    );
}
export async function LoadSongs() {
    const docs = await db.allDocs({ include_docs: true, attachments: true, binary: true, });
    return {
        count: docs.rows,
        docs: docs.rows.map(row => ({ ...row.doc, song: row.doc?.attachments?.song, attachments: undefined }))
    };

}